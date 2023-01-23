UINT  _nxe_web_http_server_param_get(NX_PACKET *packet_ptr, UINT param_number, CHAR *param_ptr, UINT *param_size, UINT max_param_size)
{

UINT    status;


    /* Check for invalid input pointers.  */
    if ((packet_ptr == NX_NULL) || (param_ptr == NX_NULL) || (param_size == NX_NULL))
        return(NX_PTR_ERROR);

    /* Check for appropriate caller.  */
    NX_THREADS_ONLY_CALLER_CHECKING

    /* Call actual server parameter get function.  */
    status =  _nx_web_http_server_param_get(packet_ptr, param_number, param_ptr, param_size, max_param_size);

    /* Return completion status.  */
    return(status);
}