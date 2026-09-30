# Reflection

## Start

I need to start by writing the test before I change the split math. A bill that looks fine can still be a penny off, and I only noticed that when a $10.00 total became $9.99 after rounding. I also need to start checking that the finished shares add back to the grand total every time, including tax and tip.

## Stop

I need to stop treating rounding as one call to `Math.Round`. Each share can be correct on its own and the group can still be short a cent. I also need to stop putting every rule in the same method. Negative tax, a person left off the bill, and the actual split are different jobs, and mixing them made the code harder to follow.

## Continue

I will keep one class for each job: the bill, the people, the rounder, the validator, and the receipt. Short comments that say what the next step does are enough. I will also keep making one small commit for each behavior, with a test next to it, so I can explain what changed without rereading the whole program.

The part I understand better now is that a passing test is not only "the number looks right." The shares have to add up, the bad inputs have to be rejected, and two tests are still supposed to fail because CSV export and card payments are not part of this assignment yet.
