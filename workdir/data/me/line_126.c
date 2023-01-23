UINT  _nxe_web_http_server_content_get(NX_WEB_HTTP_SERVER *server_ptr, NX_PACKET *packet_ptr, ULONG byte_offset, CHAR *destination_ptr, UINT destination_size, UINT *actual_size)
{

UINT    status;


    /* Check for invalid input pointers.  */
    if ((server_ptr == NX_NULL) || (server_ptr -> nx_web_http_server_id != NX_WEB_HTTP_SERVER_ID) ||
        (packet_ptr == NX_NULL) || (destination_ptr == NX_NULL) || (actual_size == NX_NULL))
        return(NX_PTR_ERROR);

    /* Check for appropriate caller.  */
    NX_THREADS_ONLY_CALLER_CHECKING

    /* Call actual server content get function.  */
    status =  _nx_web_http_server_content_get(server_ptr, packet_ptr, byte_offset, destination_ptr, destination_size, actual_size);

    /* Return completion status.  */
    return(status);
}