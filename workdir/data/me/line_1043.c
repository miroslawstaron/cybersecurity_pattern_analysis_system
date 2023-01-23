UINT  _nxe_web_http_server_content_length_get(NX_PACKET *packet_ptr, ULONG *content_length)
{

UINT    status;


    /* Check for invalid input pointers.  */
    if ((packet_ptr == NX_NULL) || (content_length == NX_NULL))
        return(NX_PTR_ERROR);

    /* Call actual server content length get function.  */
    status =  _nx_web_http_server_content_length_get(packet_ptr, content_length);

    /* Return completion status. */
    return status;
}