public final class AccountNumber {
private final String value;
public AccountNumber(String value) {
if(!isValid(value)){
throw new IllegalArgumentException("Invalid account number");
}
this.value = value;
}
public static boolean isValid(String accountNumber){
return accountNumber != null && hasLength(accountNumber, 10, 12) && isNumeric(accountNumber); 
}
}
