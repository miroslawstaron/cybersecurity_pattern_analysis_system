UINT  _nx_web_http_server_field_value_get(NX_PACKET *packet_ptr, UCHAR *field_name, ULONG name_length, UCHAR *field_value, ULONG field_value_size)
{

UCHAR  *ch;
UINT    index;


    /* Initialize. */
    ch = packet_ptr -> nx_packet_prepend_ptr;

    /* Loop to find field name. */
    while(ch + name_length < packet_ptr -> nx_packet_append_ptr)
    {
        if(_nx_web_http_server_memicmp(ch, name_length, field_name, name_length) == NX_SUCCESS)
        {

            /* Field name found. */
            break;
        }

        /* Move the pointer up to the next character.  */
        ch++;
    }

    /* Skip field name and ':'. */
    ch += name_length + 1;

    /* Is field name found? */
    if(ch >= packet_ptr -> nx_packet_append_ptr)
        return NX_WEB_HTTP_NOT_FOUND;

    /* Skip white spaces. */
    while(*ch == ' ')
    {
        if(ch >= packet_ptr -> nx_packet_append_ptr)
            return NX_WEB_HTTP_NOT_FOUND;

        /* Get next character. */
        ch++;
    }

    /* Copy field value. */
    index = 0;
    while(index < field_value_size)
    {

        /* Whether the end of line CRLF is not found? */
        if(ch + 2 > packet_ptr -> nx_packet_append_ptr)
            return NX_WEB_HTTP_NOT_FOUND;

        /* Compare CRLF. */ 
        if((*ch == 13) && (*(ch + 1) == 10))
            break;

        /* Copy data. */
        field_value[index++] = *ch++;
    }

    /* Buffer overflow? */
    if(index == field_value_size)
        return NX_WEB_HTTP_NOT_FOUND;

    /* Trim white spaces. */
    while((index > 0) && (field_value[index - 1] == ' '))
        index--;

    /* Append terminal 0. */
    field_value[index] = 0;


    return NX_SUCCESS;
}