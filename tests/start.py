'''
This is the basic test file
'''

def r12(a):
  return a

def rev_str(strA):
  return strA [::-1]

def rev_str(strA):
  return strA [::-1]

# check if a year is a leap year
def check_leap_year(intYear):
  if (intYear % 4 == 0) and (intYear % 100 != 0):
    if (intYear % 400 == 0):
      return True
  elif (intYear % 4 != 0):
      return False
  elif (intYear % 400 == 0):  
    return True
  
  return False