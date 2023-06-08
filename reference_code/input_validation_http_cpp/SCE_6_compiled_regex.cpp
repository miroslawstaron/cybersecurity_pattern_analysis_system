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