UINT  _nx_web_http_server_content_length_get(NX_PACKET *packet_ptr, ULONG *length)
{

CHAR    *buffer_ptr;


    /* Default the content length to no data.  */
    *length =  0;

    /* Setup pointer to buffer.  */
    buffer_ptr =  (CHAR *) packet_ptr -> nx_packet_prepend_ptr;

    /* Find the "Content-length: " token first.  */
    while (((buffer_ptr+17) < (CHAR *) packet_ptr -> nx_packet_append_ptr) && (*buffer_ptr != (CHAR) 0))
    {

        /* Check for the Content-length token.  */
        if (((*buffer_ptr ==      'c') || (*buffer_ptr ==      'C')) &&
            ((*(buffer_ptr+1) ==  'o') || (*(buffer_ptr+1) ==  'O')) &&
            ((*(buffer_ptr+2) ==  'n') || (*(buffer_ptr+2) ==  'N')) &&
            ((*(buffer_ptr+3) ==  't') || (*(buffer_ptr+3) ==  'T')) &&
            ((*(buffer_ptr+4) ==  'e') || (*(buffer_ptr+4) ==  'E')) &&
            ((*(buffer_ptr+5) ==  'n') || (*(buffer_ptr+5) ==  'N')) &&
            ((*(buffer_ptr+6) ==  't') || (*(buffer_ptr+6) ==  'T')) &&
            (*(buffer_ptr+7) ==  '-') &&
            ((*(buffer_ptr+8) ==  'l') || (*(buffer_ptr+8) ==  'L')) &&
            ((*(buffer_ptr+9) ==  'e') || (*(buffer_ptr+9) ==  'E')) &&
            ((*(buffer_ptr+10) == 'n') || (*(buffer_ptr+10) == 'N')) &&
            ((*(buffer_ptr+11) == 'g') || (*(buffer_ptr+11) == 'G')) &&
            ((*(buffer_ptr+12) == 't') || (*(buffer_ptr+12) == 'T')) &&
            ((*(buffer_ptr+13) == 'h') || (*(buffer_ptr+13) == 'H')) &&
            (*(buffer_ptr+14) == ':') &&
            (*(buffer_ptr+15) == ' '))
        {

            /* Move the pointer up to the length token.  */
            buffer_ptr =  buffer_ptr + 16;
            break;
        }

        /* Move the pointer up to the next character.  */
        buffer_ptr++;
    }

    /* Now convert the length into a numeric value.  */
    while ((buffer_ptr < (CHAR *) packet_ptr -> nx_packet_append_ptr) && (*buffer_ptr >= '0') && (*buffer_ptr <= '9'))
    {

        /* Update the content length.  */
        *length =  *length * 10;
        *length =  *length + (((UINT) (*buffer_ptr)) - 0x30);

        /* Move the buffer pointer forward.  */
        buffer_ptr++;
    }

     /* Determine if the content length was picked up properly.  */
     if ((buffer_ptr >= (CHAR *) packet_ptr -> nx_packet_append_ptr) ||
         ((*buffer_ptr != ' ') && (*buffer_ptr != (CHAR) 13)))
     {

         /* Error, set the length to zero.  */
         return NX_WEB_HTTP_INCOMPLETE_PUT_ERROR;
     }

    /* Return successful completion status to the caller.  */
    return NX_SUCCESS;
}