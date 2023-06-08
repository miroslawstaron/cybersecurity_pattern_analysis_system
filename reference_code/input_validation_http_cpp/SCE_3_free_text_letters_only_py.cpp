def only_letters(s):
    """
    Returns True if the input text consists of letters and ideographs only, False otherwise.
    """
    for c in s:
        cat = unicodedata.category(c)
        # Ll=lowercase, Lu=uppercase, Lo=ideographs
        if cat not in ('Ll','Lu','Lo'):
            return False
    return True
 
> only_letters('Bzdrężyło')
True
> only_letters('He7lo') # we don't allow digits here
False