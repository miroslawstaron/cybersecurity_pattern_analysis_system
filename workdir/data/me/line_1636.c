UINT  _nx_web_http_server_param_get(NX_PACKET *packet_ptr, UINT param_number, CHAR *param_ptr, UINT *param_size, UINT max_param_size)
{

UINT    i;
UINT    current_param;
CHAR    *buffer_ptr;


    /* Set the destination string to NULL.  */
    param_ptr[0] =  (CHAR) NX_NULL;
    *param_size = 0;

    /* Set current parameter to 0.  */
    current_param =  0;

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

    /* Loop through the buffer to search for the specified parameter.  */
    do
    {

        /* Determine if the character is a semicolon, indicating a parameter
           is present.  */
        if (*buffer_ptr == ';')
        {

            /* Yes, a parameter is present.  */

            /* Move the buffer pointer forward.  */
            buffer_ptr++;

            /* Is this the parameter requested?  */
            if (current_param == param_number)
            {


                /* Yes, we have found the parameter.  */
                for (i = 0; i < max_param_size; i++)
                {

                    /* Check if reach the end of the packet data.  */
                    if (buffer_ptr >= (CHAR *)packet_ptr -> nx_packet_append_ptr)
                    {
                        return(NX_WEB_HTTP_NOT_FOUND);
                    }

                    /* Check for end of parameter.  */
                    if ((*buffer_ptr == ';') || (*buffer_ptr == '?') ||
                        (*buffer_ptr == '&') || (*buffer_ptr == ' ') ||
                        (*buffer_ptr == (CHAR) 13))
                    {

                        /* Yes, we are finished and need to get out of the loop.  */
                        break;
                    }

                    /* Otherwise, store the character in the destination.  */
                    param_ptr[i] =  *buffer_ptr++;
                }

                /* NULL terminate the parameter.  */
                if (i < max_param_size)
                {
                    param_ptr[i] =  (CHAR) NX_NULL;
                }

                /* Return to caller.  */
                if (param_ptr[i] == (CHAR) NX_NULL)
                {
                    *param_size = i;
                    return(NX_SUCCESS);
                }
                else
                {
                    return(NX_WEB_HTTP_IMPROPERLY_TERMINATED_PARAM);
                }
            }
            else
            {

                /* Increment the current parameter.  */
                current_param++;
            }
        }
        else
        {

            /* Check for any other character that signals the end of the param list.  */
            if ((*buffer_ptr == '?') || (*buffer_ptr == ' ') || (*buffer_ptr == '&'))
                break;

            /* Update the buffer pointer.  */
            buffer_ptr++;
        }

    } while ((buffer_ptr < (CHAR *) packet_ptr -> nx_packet_append_ptr) && (*buffer_ptr != (CHAR) 13));

    /* Return a not found error.  */
    return(NX_WEB_HTTP_NOT_FOUND);
}