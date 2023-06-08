     int testScore;
     cout << "Enter test score" << endl;
     cin >> testScore;
     if (testScore >= 90)
	cout << "Your grade is A" << endl;
     else if (testScore >= 80)
	cout << "Your grade is B" << endl;
     else if (testScore >= 70)
	cout << "Your grade is C" << endl;
     else if (testScore >= 60)
	cout << "Your grade is D" << endl;
     else
	cout << "Your grade is F" << endl;

     return 0;