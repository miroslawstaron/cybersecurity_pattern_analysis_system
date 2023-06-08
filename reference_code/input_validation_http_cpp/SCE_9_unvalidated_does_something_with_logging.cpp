public void Foo(string untrusted_bar)
{
if (!IsValid(untrusted_bar))
{
throw new ValidationException();
}
var bar = untrusted_bar;
DoSomethingWith(bar);
}
