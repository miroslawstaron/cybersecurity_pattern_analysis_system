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
}