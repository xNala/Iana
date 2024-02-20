# Iana

2/19/2024 Public Release:

--Note: this code was just a proof of concept.

Iana is a primitive usermode "anti-virus" which relies on C#'s FileSystemWatcher (as far as I can tell, this relies on ReadDirectoryChangesW from the WinAPI).

Any event is passed to YARA, and the results are handled.


This setup sucks, and is just a concept.

If you're going to use this in the real world, you should look at going lower and using something more reliable than ReadDirectoryChangesW.
