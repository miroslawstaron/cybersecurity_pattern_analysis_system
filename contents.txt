// compile regex expressions 
res = regcomp(pattern, “[abc]{1,3}", REG_EXTENDED|REG_NOSUB); 
// Use compiled regex to test input value “aa” 
res = regexec(pattern, “aa” , 0, NULL, 0); 
if (res) printf(“aa matched\n”); 
 else printf(“aa failed\n”); 
// Use compiled regex to test input value “ad” 
res = regexec(pattern, “ad” , 0, NULL, 0); 
if (res) printf(“ad matched\n”); 
 else printf(“ad failed\n”);
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
UINT  _nx_web_http_server_type_get(NX_WEB_HTTP_SERVER *server_ptr, CHAR *name, CHAR *http_type_string, UINT *string_size)
{
UINT    temp_name_length;

    /* Check name length.  */
    if (_nx_utility_string_length_check(name, &temp_name_length, NX_MAX_STRING_LENGTH))
    {
        return(NX_WEB_HTTP_ERROR);
    }

    /* Call actual service type get function. */
    return(_nx_web_http_server_type_get_extended(server_ptr, name, temp_name_length,
                                                 http_type_string, NX_MAX_STRING_LENGTH + 1,
                                                 string_size));
}
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
UINT  _nxe_web_http_server_packet_get(NX_WEB_HTTP_SERVER *server_ptr, NX_PACKET **packet_ptr)
{


    /* Check for invalid packet pointer.  */
    if((server_ptr == NX_NULL) || (server_ptr -> nx_web_http_server_id != NX_WEB_HTTP_SERVER_ID) ||
       (packet_ptr == NX_NULL))
        return(NX_PTR_ERROR);

    /* Call actual server packet get function.  */
    return(_nx_web_http_server_packet_get(server_ptr, packet_ptr));

}
Integer MakeChoice(String s) { 
 x = Integer.parseInt(s); 
 if (x < 1 || x > 3) { 
 throw new NumberFormatException 
 ("Value of must be between 1 and 3"); 
 } 
 return x; 
}
public final class AccountNumber {
private final String value;
public AccountNumber(String value) {
if(!isValid(value)){
throw new IllegalArgumentException("Invalid account number");
}
this.value = value;
public static boolean isValid(String accountNumber){
return accountNumber != null && hasLength(accountNumber, 10, 12) && isNumeric(accountNumber); 
#include <stdio.h> 
int main(int argc, char * argv[]) { 
printf(“%s\n”, argv[0]); 
exit(1); 
} 

// gcc -o argtest argtest.c 

// When run normally 
// /home/rritchey$ argtest 

// Argtest 
// But attacker can change arg0 
// when calling execl so 

execl(“argtest”, “blah”, NULL) 
// blah
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

        /* Receive another packet from client.  */
        status = _nx_web_http_server_receive(server_ptr, &new_packet_ptr, NX_WEB_HTTP_SERVER_TIMEOUT_RECEIVE);

    /* Check the return status.  */
    if (status != NX_SUCCESS)

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
    
    *packet_ptr = new_packet_ptr;

    return(NX_SUCCESS);

}
     int testScore;
     cout << "Enter test score" << endl;
     cin >> testScore;
     if (testScore >= 90)
	cout << "Your grade is A" << endl;
     else if (testScore >= 80)
	cout << "Your grade is B" << endl;
     else if (testScore >= 70)
	cout << "Your grade is C" << endl;
     else if (testScore >= 60)
	cout << "Your grade is D" << endl;
     else
	cout << "Your grade is F" << endl;

     return 0;
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
import semmle.code.java.dataflow.FlowSources

class SpringServletInputParameterSource extends RemoteFlowSource {
    SpringServletInputParameterSource() {
      this.asParameter() = any(SpringRequestMappingParameter srmp | 
                                                    srmp.isTaintedInput())
    }

    override string getSourceType() {
        result = "Spring servlet input parameter" }
  }

from SpringServletInputParameterSource c
where not c.asParameter()
                   .getAnAnnotation()
                   .getType()
                   .hasQualifiedName("javax.validation", "Valid")
select c
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
        return(NX_WEB_HTTP_NOT_FOUND);

    /* Loop through the buffer to search for the specified query instance.  */
    do

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

                        /* Yes, we are finished and need to get out of the loop.  */
                        break;

                    /* Otherwise, store the character in the destination.  */
                    query_ptr[i] =  *buffer_ptr++;
                }

                /* NULL terminate the query.  */
                query_ptr[i] =  (CHAR) NX_NULL;

                /* Return to caller.  */
                if (i)
                    *query_size = i;
                    return(NX_SUCCESS);
                else
                    return(NX_WEB_HTTP_NO_QUERY_PARSED);
            }
            else

                /* Increment the current query.  */
                current_query++;
        }
        else

            /* Check for any other character that signals the end of the query list.  */
            if ((*buffer_ptr == '?') || (*buffer_ptr == ' ') || (*buffer_ptr == ';'))
                break;

            /* Update the buffer pointer.  */

    } while ((buffer_ptr < (CHAR *) packet_ptr -> nx_packet_append_ptr) && (*buffer_ptr != (CHAR) 13));

    /* Return a not found error.  */
    return(NX_WEB_HTTP_NOT_FOUND);
}
UINT  _nx_web_http_server_content_get(NX_WEB_HTTP_SERVER *server_ptr, NX_PACKET *packet_ptr, ULONG byte_offset, CHAR *destination_ptr, UINT destination_size, UINT *actual_size)
{
UINT status;

    /* Get content data.  */
    status = _nx_web_http_server_content_get_extended(server_ptr, packet_ptr, byte_offset, destination_ptr, destination_size, actual_size);

    return(status);
}
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
        return(NX_WEB_HTTP_NOT_FOUND);

    /* Loop through the buffer to search for the specified parameter.  */
    do

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

                        /* Yes, we are finished and need to get out of the loop.  */
                        break;

                    /* Otherwise, store the character in the destination.  */
                    param_ptr[i] =  *buffer_ptr++;
                }

                /* NULL terminate the parameter.  */
                if (i < max_param_size)
                    param_ptr[i] =  (CHAR) NX_NULL;

                /* Return to caller.  */
                if (param_ptr[i] == (CHAR) NX_NULL)
                    *param_size = i;
                    return(NX_SUCCESS);
                else
                    return(NX_WEB_HTTP_IMPROPERLY_TERMINATED_PARAM);
            }
            else

                /* Increment the current parameter.  */
                current_param++;
        }
        else

            /* Check for any other character that signals the end of the param list.  */
            if ((*buffer_ptr == '?') || (*buffer_ptr == ' ') || (*buffer_ptr == '&'))
                break;

            /* Update the buffer pointer.  */

    } while ((buffer_ptr < (CHAR *) packet_ptr -> nx_packet_append_ptr) && (*buffer_ptr != (CHAR) 13));

    /* Return a not found error.  */
    return(NX_WEB_HTTP_NOT_FOUND);
}
public void tryTransfer(Amount amount) {
if (!this.account.contains(amount)) {
throw new ValidationException();
}
transfer(amount);
}
UINT  _nxe_web_http_server_content_get(NX_WEB_HTTP_SERVER *server_ptr, NX_PACKET *packet_ptr, ULONG byte_offset, CHAR *destination_ptr, UINT destination_size, UINT *actual_size)
{

UINT    status;


    /* Check for invalid input pointers.  */
    if ((server_ptr == NX_NULL) || (server_ptr -> nx_web_http_server_id != NX_WEB_HTTP_SERVER_ID) ||
        (packet_ptr == NX_NULL) || (destination_ptr == NX_NULL) || (actual_size == NX_NULL))
        return(NX_PTR_ERROR);

    /* Check for appropriate caller.  */
    NX_THREADS_ONLY_CALLER_CHECKING

    /* Call actual server content get function.  */
    status =  _nx_web_http_server_content_get(server_ptr, packet_ptr, byte_offset, destination_ptr, destination_size, actual_size);

    /* Return completion status.  */
    return(status);
}
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

        /* Update the content length.  */
        *length =  *length * 10;
        *length =  *length + (((UINT) (*buffer_ptr)) - 0x30);

        /* Move the buffer pointer forward.  */

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
void escape_shell_cmd(char *cmd) { 
 register int x,y,l; 
 l=strlen(cmd); 
 for(x=0;cmd[x];x++) { 
 if(ind("&;`'\"|*?~<>^()[]{}$\\",cmd[x]) != -1){ 
 for(y=l+1;y>x;y-- 
 cmd[y] = cmd[y-1]; 
 l++; /* length has been increased */ 
 cmd[x] = '\\'; 
 x++; /* skip the character */ 
 } 
}
public class SQLInjectionValidator 
{
 public static String validateInput(String input)
 {
 return input.replaceAll("select", "%select%");
 }
}

public class main 
 public static void main(String[] args) 
 String userName = "select * from table";
 String password = "fakePassword";
 
 userName = SQLInjectionValidator.validateInput(userName);
 password = SQLInjectionValidator.validateInput(password);
 
 System.out.print("Sanitized User: " + userName + "\n");
 System.out.print("Sanitized Password: " + password);

}
public class Optional<T> {
public bool IsPresent();
public T Get();
}

int *foo = null;
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
                continue;

            /* Calculate the size.  */
            if ((tmp >= 'a') && (tmp <= 'f'))
                size = (size << 4) + 10 + (UINT)(tmp - 'a');
            else if ((tmp >= 'A') && (tmp <= 'F'))
                size = (size << 4) + 10 + (UINT)(tmp - 'A');
            else if ((tmp >= '0') && (tmp <= '9'))
                size = (size << 4) + (UINT)(tmp - '0');
            else if (tmp == '\r')

                /* Find the end of chunk header.  */
                break;
            else if (tmp == ';')

                /* Find chunk extension.  */
                chunk_extension = 1;
            else
                return(NX_NOT_FOUND);
        }

        /* Expect '\n'.  */
        status = _nx_web_http_server_request_byte_expect(server_ptr, '\n', wait_option, current_packet_pptr, current_data_pptr);
        if (status)
            return(status);

        *chunk_size = size;

    /* If there is no remaining data, receive another packet.  */
    while (server_ptr -> nx_web_http_server_chunked_request_remaining_size == 0)
        if (server_ptr -> nx_web_http_server_request_packet)
            nx_packet_release(server_ptr -> nx_web_http_server_request_packet);

        status = _nx_web_http_server_receive(server_ptr, &(server_ptr -> nx_web_http_server_request_packet), wait_option);

        /* Update the current request packet, data pointer and remaining size.  */
        (*current_packet_pptr) = server_ptr -> nx_web_http_server_request_packet;
        (*current_data_pptr) = server_ptr -> nx_web_http_server_request_packet -> nx_packet_prepend_ptr;
        server_ptr -> nx_web_http_server_chunked_request_remaining_size = server_ptr -> nx_web_http_server_request_packet -> nx_packet_length;

    return(NX_SUCCESS);
}
String MakeChoice(String s) { 
 if (s.equalsIgnoreCase(“File”) return s; 
 if (s.equalsIgnoreCase(“Edit”) return s; 
 if (s.equalsIgnoreCase(“View”) return s; 
 throw new StringFormatException 
 ("Value must be either File, Edit, or 
View"); 
 } 
}
private static final Pattern zipPattern = Pattern.compile("^\d{5}(-\d{4})?$");

public void doPost( HttpServletRequest request, HttpServletResponse response) {
  try {
      String zipCode = request.getParameter( "zip" );
      if ( !zipPattern.matcher( zipCode ).matches()  {
          throw new YourValidationException( "Improper zipcode format." );
      }
      // do what you want here, after its been validated ..
  } catch(YourValidationException e ) {
      response.sendError( response.SC_BAD_REQUEST, e.getMessage() );
  }
}
public class Amount {
private final Integer value;
public Amount(Integer value) {
if (!isValid(value) {
throw new IllegalArgumentException(); 
}
this.value = value;
public Integer getValue() {
return this.value;
{
    var message="";
    // Specify validation requirements for different fields.
    Validation.RequireField("coursename", "Class name is required");
    Validation.RequireField("credits", "Credits is required");
    Validation.Add("coursename", Validator.StringLength(5));
    Validation.Add("credits", Validator.Integer("Credits must be an integer"));
    Validation.Add("credits", Validator.Range(1, 5, "Credits must be between 1 and 5"));
    Validation.Add("startDate", Validator.DateTime("Start date must be a date"));

    if (IsPost)  {
        // Before processing anything, make sure that all user input is valid.
        if (Validation.IsValid()) {
            var coursename = Request["coursename"];
            var credits = Request["credits"].AsInt();
            var startDate = Request["startDate"].AsDateTime();
            message += @"For Class, you entered " + coursename;
            message += @"<br/>For Credits, you entered " + credits.ToString();
            message += @"<br/>For Start Date, you entered " + startDate.ToString("dd-MMM-yyyy");

            // Further processing here
        }
    }
}
<!DOCTYPE html>
<html lang="en">
<head>
  <title>Validation Example</title>
  <style>
      body {margin: 1in; font-family: 'Segoe UI'; font-size: 11pt; }
   </style>
</head>
<body>
  <h1>Validation Example</h1>
  <p>This example page asks the user to enter information about some classes at school.</p>
  <form method="post">
    @Html.ValidationSummary()
    <div>
      <label for="coursename">Course name: </label>
      <input type="text"
         name="coursename"
         value="@Request["coursename"]"
      />
      @Html.ValidationMessage("coursename")
    </div>

      <label for="credits">Credits: </label>
         name="credits"
         value="@Request["credits"]"
      @Html.ValidationMessage("credits")

      <label for="startDate">Start date: </label>
         name="startDate"
         value="@Request["startDate"]"
      @Html.ValidationMessage("startDate")

   <div>
      <input type="submit" value="Submit" class="submit" />

      @if(IsPost){
        <p>@Html.Raw(message)</p>
      }
  </form>
</body>
</html>
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
        if(ch >= packet_ptr -> nx_packet_append_ptr)
            return NX_WEB_HTTP_NOT_FOUND;

        /* Get next character. */

    /* Copy field value. */
    index = 0;
    while(index < field_value_size)

        /* Whether the end of line CRLF is not found? */
        if(ch + 2 > packet_ptr -> nx_packet_append_ptr)

        /* Compare CRLF. */ 
        if((*ch == 13) && (*(ch + 1) == 10))

        /* Copy data. */
        field_value[index++] = *ch++;

    /* Buffer overflow? */
    if(index == field_value_size)

    /* Trim white spaces. */
    while((index > 0) && (field_value[index - 1] == ' '))
        index--;

    /* Append terminal 0. */
    field_value[index] = 0;


    return NX_SUCCESS;
}
public void Foo(string bar)
{
if (!IsValid(bar))
throw new ValidationException();
}
DoSomethingWith(bar);
UINT  _nxe_web_http_server_type_get(NX_WEB_HTTP_SERVER *server_ptr, CHAR *name, CHAR *http_type_string, UINT *string_size)
{
UINT status;


    /* Check for invalid input pointers.  */
    if ((server_ptr == NX_NULL) || (server_ptr -> nx_web_http_server_id != NX_WEB_HTTP_SERVER_ID) ||
        (name == NX_NULL)       || (http_type_string == NX_NULL) || (string_size == NX_NULL))
    {
        return(NX_PTR_ERROR);
    }

    status = _nx_web_http_server_type_get(server_ptr, name, http_type_string, string_size);

    return(status);
}
public void Foo(string untrusted_bar)
{
if (!IsValid(untrusted_bar))
throw new ValidationException();
}
var bar = untrusted_bar;
DoSomethingWith(bar);
def only_letters(s):
    """
    Returns True if the input text consists of letters and ideographs only, False otherwise.
    for c in s:
        cat = unicodedata.category(c)
        # Ll=lowercase, Lu=uppercase, Lo=ideographs
        if cat not in ('Ll','Lu','Lo'):
            return False
    return True
 
> only_letters('Bzdrężyło')
True
> only_letters('He7lo') # we don't allow digits here
False