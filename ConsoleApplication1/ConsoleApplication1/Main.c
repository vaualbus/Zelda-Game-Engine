#include <stdio.h>
#include <stdlib.h>

#define N 5

void Test1()
{
	int vet[N] = { 0 };
	int min;

	for (int i = 0; i < N; i++)
	{
		printf("Insersci l'element %d-%d: ", i + 1, N);
		scanf_s("%d", &vet[i]);
	}

	int decr = 0;
	int index = -1;
	min = vet[0];
	for (int i = 3; i < N && decr == 0; i++)
	{
		if (vet[i] > vet[i - 1] && vet[i] < vet[i + 1])
		{
			decr = 1;
			index = i;
		}
	}

	if (decr)
		printf("Gli elementi sono: %d %d %d\n", vet[index - 1], vet[index], vet[index + 1]);
	else
		printf("Gli elementi sono random, o al massimo non ci sono tre elementi decrescenti consecutivi!");
}

void PrintVector(int *vet)
{
	for (int i = 0; i < N; i++)
		printf("%d ", vet[i]);
}

int main()
{
	
	int vet1[N] = { 0 };
	int vet2[N] = { 0 };

	int counter = 0;
	int flag = 1;

	for (int i = 0; i < N; )
	{
		printf("Inserisci Un elemento: (%d di %d) ", i, N - 1);
		int tempValue;
		scanf_s("%d", &tempValue);

		if (tempValue >= 0 && (tempValue % 3) == 0)
		{
			vet1[i] = tempValue;
			i++;
		}
		else if (tempValue < 0 && (tempValue % 3) != 0)
		{
			vet2[i] = tempValue;
			i++;
		}
		else
			i++;
	}
	
	printf("Il vettore 1 contetiene: ");
	PrintVector(vet1);
	printf("\n");

	printf("Il vettore 2 contetiene: ");
	PrintVector(vet2);
	printf("\n");

	system("PAUSE");

	return 0;
}