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