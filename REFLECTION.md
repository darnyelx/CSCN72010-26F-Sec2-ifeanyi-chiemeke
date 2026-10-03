# Reflection

Writing tests before working on the calculations makes it easier to catch problems early. At first, I assumed the split was correct as long as each person’s share looked close enough. When I split a $10 bill between three people, the total came out to $9.99. That helped me understand why the leftover cent needs to be accounted for. The tests need to confirm that all the individual shares add back up to the full bill, including tax and tip.

I also learned that using `Math.Round` alone does not completely solve the rounding problem. Each individual share can be rounded correctly while the final total is still off by a cent. Validation and bill-splitting logic also work better when they are kept separate. A negative tax value is an input problem, while leaving someone out of the bill is a splitting problem. Separating these responsibilities makes problems easier to find and fix.

Keeping the bill, people, rounding, validation, and receipt responsibilities in separate classes made the code easier to understand. Short comments were useful when returning to the code later, and small commits made it easier to see what had changed without going through everything again.

The biggest thing I learned is that a green test does not simply mean that the number looks right. The individual shares must add up to the total, invalid input must be rejected, and the CSV and Stripe tests are intentionally still red because those features are outside the scope of this assignment.