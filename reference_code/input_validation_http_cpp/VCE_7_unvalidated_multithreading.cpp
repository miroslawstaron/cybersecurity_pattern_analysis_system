public void tryTransfer(Amount amount) {
if (!this.account.contains(amount)) {
throw new ValidationException();
}
transfer(amount);
}