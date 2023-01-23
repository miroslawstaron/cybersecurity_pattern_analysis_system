UINT  _nxe_web_http_server_query_get(NX_PACKET *packet_ptr, UINT query_number, CHAR *query_ptr, UINT *query_size, UINT max_query_size)
{

UINT    status;


    /* Check for invalid input pointers.  */
    if ((packet_ptr == NX_NULL) || (query_ptr == NX_NULL) || (query_size == NX_NULL))
        return(NX_PTR_ERROR);

    /* Check for appropriate caller.  */
    NX_THREADS_ONLY_CALLER_CHECKING

    /* Call actual server query get function.  */
    status =  _nx_web_http_server_query_get(packet_ptr, query_number, query_ptr, query_size, max_query_size);

    /* Return completion status.  */
    return(status);
}