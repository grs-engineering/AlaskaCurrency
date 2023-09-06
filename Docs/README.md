# Alaska Airlines Interview - Currency Converter API

## Problem Definition
We've been asked to create a currency conversion web service endpoint.

### Details 
* The endpoint should take FromCountry, ToCountry, and Amount, and return a converted Amount. 
* Our consumers are varied, including an internal accounting system and our website's searched flight pricing.
* This web service will be scaled horizontally. 
* Leadership is OK with eventual consistency of the conversion rate data on each node. If currency rates in some nodes lag a few minutes behind latest currency rates in other nodes, that's OK.

### Details of Team Who Will Own This Service
* Our consumers mostly use REST web services.
* We mostly uses .NET. 
* We strive to use the latest stable tech where applicable. 
* We lean on HTTP standards where possible.

### Conversion Rate Data Source
Our currency rates come from a csv file containing CountryCode, CurrencyName, and RateFromUSDToCurrency. This csv file is located on the file system, and updated by another process about once per hour. Included is an example csv. We're hoping to switch to a third party service to get our conversion rates later.

### Out of scope
* Load balancing and other networking
* Authorization and Authentication

## How to submit
Submit a PR to your private repo and invite the github users provided by your recruiter as collaborators.

On your PR, feel free to add comments such as:
* Any follow up questions you would have asked business or other devs on the team?
* Decisions you made in lieu of more clear requirements.
* Enhancements you'd make later, given more time.
