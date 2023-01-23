UINT  _nx_web_http_server_query_get(NX_PACKET *packet_ptr, UINT query_number, CHAR *query_ptr, UINT *query_size, UINT max_query_size)
{

UINT    i;
UINT    current_query;
CHAR    *buffer_ptr;


    /* Set the destination string to NULL.  */
    query_ptr[0] =  (CHAR) NX_NULL;
    *query_size = 0;

    /* Set current query number to 0.  */
    current_query =  0;

    /* Setup a pointer to the HTTP buffer.  */
    buffer_ptr =  (CHAR *) packet_ptr -> nx_packet_prepend_ptr;

    /* Position to the start of the URL.  */
    while ((buffer_ptr < (CHAR *) packet_ptr -> nx_packet_append_ptr) && (*buffer_ptr != '/'))
    {

        /* Move the buffer pointer.  */
        buffer_ptr++;
    }

    /* Not find URL.  */
    if (buffer_ptr >= (CHAR *) packet_ptr -> nx_packet_append_ptr)
    {
        return(NX_WEB_HTTP_NOT_FOUND);
    }

    /* Loop through the buffer to search for the specified query instance.  */
    do
    {

        /* Determine if the character is a '?' or a '&', indicating a query
           is present.  */
        if (((*buffer_ptr == '?') && (current_query == 0)) ||
            ((*buffer_ptr == '&') && (current_query != 0)))
        {

            /* Yes, a query is present.  */

            /* Move the buffer pointer forward.  */
            buffer_ptr++;

            /* Is this the query requested?  */
            if (current_query == query_number)
            {


                /* Yes, we have found the query.  */
                for (i = 0; i < max_query_size; i++)
                {

                    /* Check if reach the end of the packet data.  */
                    if (buffer_ptr >= (CHAR *)packet_ptr -> nx_packet_append_ptr)
                    {
                        return(NX_WEB_HTTP_NOT_FOUND);
                    }

                    /* Check for end of query.  */
                    if ((*buffer_ptr == ';') || (*buffer_ptr == '?') ||
                        (*buffer_ptr == '&') || (*buffer_ptr == ' ') ||
                        (*buffer_ptr == (CHAR) 13))
                    {

                        /* Yes, we are finished and need to get out of the loop.  */
                        break;
                    }

                    /* Otherwise, store the character in the destination.  */
                    query_ptr[i] =  *buffer_ptr++;
                }

                /* NULL terminate the query.  */
                query_ptr[i] =  (CHAR) NX_NULL;

                /* Return to caller.  */
                if (i)
                {
                    *query_size = i;
                    return(NX_SUCCESS);
                }
                else
                {
                    return(NX_WEB_HTTP_NO_QUERY_PARSED);
                }
            }
            else
            {

                /* Increment the current query.  */
                current_query++;
            }
        }
        else
        {

            /* Check for any other character that signals the end of the query list.  */
            if ((*buffer_ptr == '?') || (*buffer_ptr == ' ') || (*buffer_ptr == ';'))
                break;

            /* Update the buffer pointer.  */
            buffer_ptr++;
        }

    } while ((buffer_ptr < (CHAR *) packet_ptr -> nx_packet_append_ptr) && (*buffer_ptr != (CHAR) 13));

    /* Return a not found error.  */
    return(NX_WEB_HTTP_NOT_FOUND);
}