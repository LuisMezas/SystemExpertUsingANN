# SystemExpertUsingANN
This project is intended for "Expert System", subject of Computer Systems Engineer at Instituto Tecnologico de Acapulco, Guerrero, México. 

We are using AForge.NET Framework, an Artificial Intelligence framework for .NET. The github link to this project is https://github.com/andrewkirillov/AForge.NET and the main page of the project http://www.aforgenet.com/framework. We'll be using this framework for the development of the Artificial Neural Network.

The problem this project is trying to solve is, evaluate info about applicants(students) that intend to get a scholarship given by an institution (In Mexico, usually that institution is a government institution). 
The applicants information regularly is about: their monthly income, age, average grade, living place (such as their town, there are certain towns in Mexico that are marked as "susceptible to violence", said that, government usually tries to help students that live in that place, with a scholarship so they can continue their studies).

## Inputs
Given this information, we will try to feed the neural network. Each of the inputs will be sent to the Input Layer of the Neural Network. As of March 27th 2018, we have the next inputs for our ANN:

1. Applicant's age
2. Average Grade. 
   - (it has to be >= 8.0, in a scale from 0 to 10, if the scale is from 0 to 100, parse it into the first scale).
3. Is a Regular Student? 
   - (Regular students = those students that haven't failed a single subject during their careers).
4. Monthly income 
   - (that money that, month by month, is earned by the student itself or their parents or relatives, this income must not be greater than 2686.14 mexican peso).
5. Is the applicant already granted by PROSPERA?
   - (PROSPERA is a government program that helps people tagged as "poor", said that, in most scholarships calls, those people that are already granted by PROSPERA are highly probable to get the scholarship).
6. Living place 
   - (such as their town. There are certain towns in Mexico that are marked as "susceptible to violence", said that, government usually tries to help students that live in that place, with a scholarship so they can continue their studies).
7. Disability 
   - (has the applicant any kind of Disability? If so, it is also highly probable to get the scholarship).

## Outputs

1. Only one, and this one will answer the next question: will the applicant get the scholarship or not? so the output will be a Boolean Value.
