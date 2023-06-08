Integer MakeChoice(String s) { 
 x = Integer.parseInt(s); 
 if (x < 1 || x > 3) { 
 throw new NumberFormatException 
 ("Value of must be between 1 and 3"); 
 } 
 return x; 
}