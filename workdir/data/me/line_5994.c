UINT  _nx_web_http_server_type_get(NX_WEB_HTTP_SERVER *server_ptr, CHAR *name, CHAR *http_type_string, UINT *string_size)
{
UINT    temp_name_length;

    /* Check name length.  */
    if (_nx_utility_string_length_check(name, &temp_name_length, NX_MAX_STRING_LENGTH))
    {
        return(NX_WEB_HTTP_ERROR);
    }

    /* Call actual service type get function. */
    return(_nx_web_http_server_type_get_extended(server_ptr, name, temp_name_length,
                                                 http_type_string, NX_MAX_STRING_LENGTH + 1,
                                                 string_size));
}
