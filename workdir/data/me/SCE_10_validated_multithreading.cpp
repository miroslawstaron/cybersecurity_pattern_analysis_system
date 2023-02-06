public class Amount {
private final Integer value;
public Amount(Integer value) {
if (!isValid(value) {
throw new IllegalArgumentException(); 
}
this.value = value;
}
public Integer getValue() {
return this.value;
}
}
