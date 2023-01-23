UINT  _nxe_web_http_server_packet_get(NX_WEB_HTTP_SERVER *server_ptr, NX_PACKET **packet_ptr)
{


    /* Check for invalid packet pointer.  */
    if((server_ptr == NX_NULL) || (server_ptr -> nx_web_http_server_id != NX_WEB_HTTP_SERVER_ID) ||
       (packet_ptr == NX_NULL))
        return(NX_PTR_ERROR);

    /* Call actual server packet get function.  */
    return(_nx_web_http_server_packet_get(server_ptr, packet_ptr));

}