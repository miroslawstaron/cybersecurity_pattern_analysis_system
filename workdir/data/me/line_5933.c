UINT  _nxe_web_http_server_type_get(NX_WEB_HTTP_SERVER *server_ptr, CHAR *name, CHAR *http_type_string, UINT *string_size)
{
UINT status;


    /* Check for invalid input pointers.  */
    if ((server_ptr == NX_NULL) || (server_ptr -> nx_web_http_server_id != NX_WEB_HTTP_SERVER_ID) ||
        (name == NX_NULL)       || (http_type_string == NX_NULL) || (string_size == NX_NULL))
    {
        return(NX_PTR_ERROR);
    }

    status = _nx_web_http_server_type_get(server_ptr, name, http_type_string, string_size);

    return(status);
}