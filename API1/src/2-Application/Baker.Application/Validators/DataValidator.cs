namespace Baker.Application.Validators
{
    public static class DataValidator
    {
        public static async Task<bool> CpfCnpjValidator(string cpfCnpj)
        {
            if (cpfCnpj.Length == 11) return await ValidatorCpf(cpfCnpj);
            else if (cpfCnpj.Length == 14) return await ValidatorCnpj(cpfCnpj);
            else return false;
        }

        private static async Task<bool> ValidatorCpf(string clienteCPF)
        {
            bool result = false;

            string[] cpfsInvalidos = new string[] { "00000000000", "11111111111", "22222222222", "33333333333", "44444444444", "55555555555", "66666666666", "77777777777", "88888888888", "99999999999" };

            if (!cpfsInvalidos.Contains(clienteCPF))
            {
                int cont = 10;
                int aux = 0;
                int i;

                if (long.TryParse(clienteCPF, out long _))
                {
                    for (i = 0; i <= 8; i++)
                    {
                        aux += int.Parse(clienteCPF[i].ToString()) * cont;
                        cont--;
                    }

                    aux *= 10;
                    aux %= 11;

                    if (aux == 10) aux = 0;

                    if (aux == int.Parse(clienteCPF[9].ToString()))
                    {
                        cont = 11;
                        aux = 0;

                        for (i = 0; i <= 9; i++)
                        {
                            aux += int.Parse(clienteCPF[i].ToString()) * cont;
                            cont--;
                        }

                        aux *= 10;
                        aux %= 11;

                        if (aux == 10) aux = 0;
                        if (aux == int.Parse(clienteCPF[10].ToString()))
                        {
                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        private static async Task<bool> ValidatorCnpj(string clienteCNPJ)
        {
            bool result = false;

            int[] pesos1 = new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] pesos2 = new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int aux = 0;
            int i;

            if (long.TryParse(clienteCNPJ, out long _))
            {
                for (i = 0; i <= 11; i++)
                {
                    aux += int.Parse(clienteCNPJ[i].ToString()) * pesos1[i];
                }

                aux %= 11;

                if (aux < 2) aux = 0;
                else aux = 11 - aux;

                if (aux == int.Parse(clienteCNPJ[12].ToString()))
                {
                    aux = 0;

                    for (i = 0; i <= 12; i++)
                    {
                        aux += int.Parse(clienteCNPJ[i].ToString()) * pesos2[i];
                    }

                    aux %= 11;

                    if (aux < 2) aux = 0;
                    else aux = 11 - aux;

                    if (aux == int.Parse(clienteCNPJ[13].ToString()))
                    {
                        result = true;
                    }
                }
            }

            return result;
        }
    }
}
