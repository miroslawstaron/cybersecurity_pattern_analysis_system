UINT _nx_web_http_server_packet_get(NX_WEB_HTTP_SERVER *server_ptr, NX_PACKET **packet_ptr)
{
NX_PACKET *new_packet_ptr;
UINT       status; 

    if (server_ptr -> nx_web_http_server_request_chunked)
    {

        /* If the request packet is chunked, remove the chunk header and get the packet which contain the chunk data.  */
        status = _nx_web_http_server_request_chunked_get(server_ptr, &new_packet_ptr, NX_WEB_HTTP_SERVER_TIMEOUT_RECEIVE);
    }
    else
    {

        /* Receive another packet from client.  */
        status = _nx_web_http_server_receive(server_ptr, &new_packet_ptr, NX_WEB_HTTP_SERVER_TIMEOUT_RECEIVE);
    }

    /* Check the return status.  */
    if (status != NX_SUCCESS)
    {

        if (server_ptr -> nx_web_http_server_request_chunked)
        {

            /* Reset the chunked info.  */
            nx_packet_release(server_ptr -> nx_web_http_server_request_packet);
            server_ptr -> nx_web_http_server_request_packet = NX_NULL;
            server_ptr -> nx_web_http_server_chunked_request_remaining_size = 0;
            server_ptr -> nx_web_http_server_request_chunked = NX_FALSE;
            return(status);
        }

        /* Error, return to caller.  */
        return(NX_WEB_HTTP_TIMEOUT);
    }
    
    *packet_ptr = new_packet_ptr;

    return(NX_SUCCESS);

}