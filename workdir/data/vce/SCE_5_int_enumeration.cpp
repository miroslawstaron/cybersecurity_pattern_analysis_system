String MakeChoice(String s) { 
 if (s.equalsIgnoreCase(“File”) return s; 
 if (s.equalsIgnoreCase(“Edit”) return s; 
 if (s.equalsIgnoreCase(“View”) return s; 
 throw new StringFormatException 
 ("Value must be either File, Edit, or 
View"); 
 } 
}