public class SQLInjectionValidator 
{
 public static String validateInput(String input)
 {
 return input.replaceAll("select", "%select%");
 }
}

public class main 
{
 public static void main(String[] args) 
 {
 String userName = "select * from table";
 String password = "fakePassword";
 
 userName = SQLInjectionValidator.validateInput(userName);
 password = SQLInjectionValidator.validateInput(password);
 
 System.out.print("Sanitized User: " + userName + "\n");
 System.out.print("Sanitized Password: " + password);

 }
}