// This nonce is stored as one of the claims in the token.
// It must be included in the response from the service.

string nonce;

var tokenGenerator = new JwtTokenGenerator();
var signedToken = tokenGenerator.CreateSignedSuperIdToken(
    "Cust12020", 
    "123123123123123123",
    out nonce);