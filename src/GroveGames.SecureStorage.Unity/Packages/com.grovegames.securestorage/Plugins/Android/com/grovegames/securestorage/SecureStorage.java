package com.grovegames.securestorage;

import android.content.Context;
import android.content.SharedPreferences;
import android.security.keystore.KeyGenParameterSpec;
import android.security.keystore.KeyProperties;
import android.util.Base64;

import java.io.File;

import java.nio.charset.StandardCharsets;
import java.security.KeyStore;

import javax.crypto.Cipher;
import javax.crypto.KeyGenerator;
import javax.crypto.SecretKey;
import javax.crypto.spec.GCMParameterSpec;

public final class SecureStorage {
    private static final String KEY_STORE = "AndroidKeyStore";
    private static final String KEY_ALIAS = "com.grovegames.securestorage";
    private static final String PREFERENCES_NAME = "com.grovegames.securestorage";
    private static final String TRANSFORMATION = "AES/GCM/NoPadding";
    private static final int IV_LENGTH = 12;
    private static final int TAG_LENGTH = 128;
    private static final String KEY_MARKER_NAME = "com.grovegames.securestorage.key";

    private final SharedPreferences preferences;
    private final File keyMarker;

    public SecureStorage(Context context) {
        Context application = context.getApplicationContext();
        preferences = application.getSharedPreferences(PREFERENCES_NAME, Context.MODE_PRIVATE);
        keyMarker = new File(application.getNoBackupFilesDir(), KEY_MARKER_NAME);
    }

    public synchronized String read(String key) throws Exception {
        String stored = preferences.getString(key, null);

        if (stored == null) {
            return null;
        }

        SecretKey secretKey = loadKey();

        if (secretKey == null) {
            if (keyMarker.exists()) {
                throw new IllegalStateException("The Keystore key is not available right now.");
            }

            preferences.edit().clear().commit();
            return null;
        }

        markKey();
        byte[] data = Base64.decode(stored, Base64.NO_WRAP);

        if (data.length <= IV_LENGTH) {
            throw new IllegalStateException("The stored value of '" + key + "' is too short to decrypt.");
        }

        Cipher cipher = Cipher.getInstance(TRANSFORMATION);
        cipher.init(Cipher.DECRYPT_MODE, secretKey, new GCMParameterSpec(TAG_LENGTH, data, 0, IV_LENGTH));
        byte[] value = cipher.doFinal(data, IV_LENGTH, data.length - IV_LENGTH);
        return new String(value, StandardCharsets.UTF_8);
    }

    public synchronized boolean write(String key, String value) {
        try {
            SecretKey secretKey = loadKey();

            if (secretKey == null) {
                if (keyMarker.exists()) {
                    return false;
                }

                preferences.edit().clear().commit();
                secretKey = createKey();
            }

            markKey();

            Cipher cipher = Cipher.getInstance(TRANSFORMATION);
            cipher.init(Cipher.ENCRYPT_MODE, secretKey);
            byte[] iv = cipher.getIV();
            byte[] encrypted = cipher.doFinal(value.getBytes(StandardCharsets.UTF_8));

            if (iv.length != IV_LENGTH) {
                return false;
            }

            byte[] data = new byte[IV_LENGTH + encrypted.length];
            System.arraycopy(iv, 0, data, 0, IV_LENGTH);
            System.arraycopy(encrypted, 0, data, IV_LENGTH, encrypted.length);
            return preferences.edit().putString(key, Base64.encodeToString(data, Base64.NO_WRAP)).commit();
        } catch (Exception exception) {
            return false;
        }
    }

    public synchronized boolean delete(String key) {
        return preferences.edit().remove(key).commit();
    }

    private void markKey() {
        try {
            keyMarker.createNewFile();
        } catch (Exception exception) {
        }
    }

    private static SecretKey loadKey() throws Exception {
        KeyStore keyStore = KeyStore.getInstance(KEY_STORE);
        keyStore.load(null);
        KeyStore.Entry entry = keyStore.getEntry(KEY_ALIAS, null);

        if (entry instanceof KeyStore.SecretKeyEntry) {
            return ((KeyStore.SecretKeyEntry) entry).getSecretKey();
        }

        return null;
    }

    private static SecretKey createKey() throws Exception {
        KeyGenerator generator = KeyGenerator.getInstance(KeyProperties.KEY_ALGORITHM_AES, KEY_STORE);
        generator.init(new KeyGenParameterSpec.Builder(KEY_ALIAS, KeyProperties.PURPOSE_ENCRYPT | KeyProperties.PURPOSE_DECRYPT)
                .setBlockModes(KeyProperties.BLOCK_MODE_GCM)
                .setEncryptionPaddings(KeyProperties.ENCRYPTION_PADDING_NONE)
                .build());
        return generator.generateKey();
    }
}
