public void Foo(string bar)
{
if (!IsValid(bar))
{
throw new ValidationException();
}
DoSomethingWith(bar);
}
