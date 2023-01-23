UINT _nx_web_http_server_chunked_size_get(NX_WEB_HTTP_SERVER *server_ptr, UINT *chunk_size, ULONG wait_option, 
                                          NX_PACKET **current_packet_pptr, UCHAR **current_data_pptr)
{
UINT status;
UINT size = 0;
UCHAR tmp;
UINT  chunk_extension = 0;

    if (server_ptr -> nx_web_http_server_actual_bytes_received < server_ptr -> nx_web_http_server_expect_receive_bytes)
    {

        /* If there are bytes need to receive, set the size need to receive as chunk size.  */
        *chunk_size = server_ptr -> nx_web_http_server_expect_receive_bytes - server_ptr -> nx_web_http_server_actual_bytes_received;
    }
    else
    {

        /* Get the chunk size.  */
        while (1)
        {

            /* Read next byte from request packet.  */
            status = _nx_web_http_server_request_read(server_ptr, &tmp, wait_option, current_packet_pptr, current_data_pptr);
            if (status)
            {
                return(status);
            }

            /* Skip the chunk extension.  */
            if (chunk_extension && (tmp != '\r'))
            {
                continue;
            }

            /* Calculate the size.  */
            if ((tmp >= 'a') && (tmp <= 'f'))
            {
                size = (size << 4) + 10 + (UINT)(tmp - 'a');
            }
            else if ((tmp >= 'A') && (tmp <= 'F'))
            {
                size = (size << 4) + 10 + (UINT)(tmp - 'A');
            }
            else if ((tmp >= '0') && (tmp <= '9'))
            {
                size = (size << 4) + (UINT)(tmp - '0');
            }
            else if (tmp == '\r')
            {

                /* Find the end of chunk header.  */
                break;
            }
            else if (tmp == ';')
            {

                /* Find chunk extension.  */
                chunk_extension = 1;
            }
            else
            {
                return(NX_NOT_FOUND);
            }
        }

        /* Expect '\n'.  */
        status = _nx_web_http_server_request_byte_expect(server_ptr, '\n', wait_option, current_packet_pptr, current_data_pptr);
        if (status)
        {
            return(status);
        }

        *chunk_size = size;
    }

    /* If there is no remaining data, receive another packet.  */
    while (server_ptr -> nx_web_http_server_chunked_request_remaining_size == 0)
    {
        if (server_ptr -> nx_web_http_server_request_packet)
        {
            nx_packet_release(server_ptr -> nx_web_http_server_request_packet);
        }

        status = _nx_web_http_server_receive(server_ptr, &(server_ptr -> nx_web_http_server_request_packet), wait_option);
        if (status)
        {
            return(status);
        }

        /* Update the current request packet, data pointer and remaining size.  */
        (*current_packet_pptr) = server_ptr -> nx_web_http_server_request_packet;
        (*current_data_pptr) = server_ptr -> nx_web_http_server_request_packet -> nx_packet_prepend_ptr;
        server_ptr -> nx_web_http_server_chunked_request_remaining_size = server_ptr -> nx_web_http_server_request_packet -> nx_packet_length;
    }

    return(NX_SUCCESS);
}