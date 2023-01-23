UINT  _nx_web_http_server_content_get(NX_WEB_HTTP_SERVER *server_ptr, NX_PACKET *packet_ptr, ULONG byte_offset, CHAR *destination_ptr, UINT destination_size, UINT *actual_size)
{
UINT status;

    /* Get content data.  */
    status = _nx_web_http_server_content_get_extended(server_ptr, packet_ptr, byte_offset, destination_ptr, destination_size, actual_size);

    return(status);
}