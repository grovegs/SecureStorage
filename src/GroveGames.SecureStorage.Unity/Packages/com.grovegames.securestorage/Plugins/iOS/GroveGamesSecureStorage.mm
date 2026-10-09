#import <Foundation/Foundation.h>
#import <Security/Security.h>

static NSMutableDictionary *GroveGamesSecureStorageQuery(const void *key, int keyLength)
{
    NSString *account = [[NSString alloc] initWithBytes:key length:(NSUInteger)keyLength encoding:NSUTF8StringEncoding];

    if (account == nil)
    {
        return nil;
    }

    NSString *service = [[NSBundle mainBundle] bundleIdentifier] ?: @"";

    return [@{
        (__bridge id)kSecClass: (__bridge id)kSecClassGenericPassword,
        (__bridge id)kSecAttrService: service,
        (__bridge id)kSecAttrAccount: account,
        (__bridge id)kSecAttrAccessible: (__bridge id)kSecAttrAccessibleAfterFirstUnlockThisDeviceOnly
    } mutableCopy];
}

extern "C" void *GroveGamesSecureStorage_Read(const void *key, int keyLength, int *valueLength)
{
    *valueLength = 0;
    NSMutableDictionary *query = GroveGamesSecureStorageQuery(key, keyLength);

    if (query == nil)
    {
        return NULL;
    }

    query[(__bridge id)kSecReturnData] = @YES;
    query[(__bridge id)kSecMatchLimit] = (__bridge id)kSecMatchLimitOne;

    CFTypeRef result = NULL;

    if (SecItemCopyMatching((__bridge CFDictionaryRef)query, &result) != errSecSuccess || result == NULL)
    {
        return NULL;
    }

    NSData *data = (__bridge_transfer NSData *)result;
    void *buffer = malloc(data.length > 0 ? data.length : 1);

    if (buffer == NULL)
    {
        return NULL;
    }

    memcpy(buffer, data.bytes, data.length);
    *valueLength = (int)data.length;
    return buffer;
}

extern "C" bool GroveGamesSecureStorage_Write(const void *key, int keyLength, const void *value, int valueLength)
{
    NSMutableDictionary *query = GroveGamesSecureStorageQuery(key, keyLength);

    if (query == nil)
    {
        return false;
    }

    NSData *data = [NSData dataWithBytes:value length:(NSUInteger)valueLength];
    OSStatus status = SecItemUpdate((__bridge CFDictionaryRef)query, (__bridge CFDictionaryRef)@{ (__bridge id)kSecValueData: data });

    if (status == errSecItemNotFound)
    {
        query[(__bridge id)kSecValueData] = data;
        status = SecItemAdd((__bridge CFDictionaryRef)query, NULL);
    }

    return status == errSecSuccess;
}

extern "C" bool GroveGamesSecureStorage_Delete(const void *key, int keyLength)
{
    NSMutableDictionary *query = GroveGamesSecureStorageQuery(key, keyLength);

    if (query == nil)
    {
        return false;
    }

    OSStatus status = SecItemDelete((__bridge CFDictionaryRef)query);
    return status == errSecSuccess || status == errSecItemNotFound;
}

extern "C" void GroveGamesSecureStorage_Free(void *buffer)
{
    free(buffer);
}
