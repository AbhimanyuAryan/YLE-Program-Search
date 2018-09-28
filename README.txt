-= YLE PROGRAM TITLE SEARCH ENGINE =-

This program fetches program titles from YLE API. 
It filters the search result by keyword(s) from user and with constant filters availability=ondemand and mediaobject=video.
Results are then filtered to leave out duplicate titles.

Shown titles are either in Finnish or Swedish, depending on which language was provided. If both are provided, Finnish is used.
Only 10 program titles are shown at first, but when the user scrolls all the way down, 10 more program titles are shown on UI.
This continues while there are more program titles to show. YLE API sets upper limit for search results so 100 program titles is 
the maximum amount of program titles there can be at once on the UI.

Search results with no hits won't print anything on the UI.

Searching is done by typing keyword(s) to the input field on top of the screen and by pressing enter or clicking anywhere on the screen or 
by using the submit button. Creating a new search wipes out the titles from the previous search. 
Search result can be scrolled with either mouse scroll or with the scroll bar on the right.





Known issues:
- Some extremely long titles are truncated from the end. (Design decision, because those super long titles are rare and there is not really 
an upper limit for the title.)
- Missing cool background.


Developer
Abhimanyu Aryan
08-2016
