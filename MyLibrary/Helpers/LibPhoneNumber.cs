using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneNumbers;


namespace MyLibrary.Helpers
{
    public static class LibPhoneNumber
    {
        public static bool IsValidPhoneNumber(string phoneNumber, bool mustBeMobileTypeOnly, string defaultRegion = "BE")
        {
            try
            {
                bool isValid ;
                //Création de l'intance PhoneNumberUtil
                var util = PhoneNumberUtil.GetInstance();
                PhoneNumber number = null;
                //Si le numéro contient l'indicatif + ou le 00
                if (phoneNumber.StartsWith("+") || phoneNumber.StartsWith("00"))
                {
                    if (phoneNumber.StartsWith("00"))
                    {
                        phoneNumber = "+" + phoneNumber.Remove(0, 2);
                    }

                    number = util.Parse(phoneNumber, "");
                    // Récupération de la région au numéro avec l'indication +
                    string regionCode = util.GetRegionCodeForNumber(number);
                    // Validation du numéro qui correspond à la région trouvée
                    isValid = util.IsValidNumberForRegion(number, regionCode);
                }
                else
                {
                    number = util.Parse(phoneNumber, defaultRegion);
                    // Validation du numéro sans indication mais avec le region code
                    isValid = util.IsValidNumber(number);
                }
                //Vérification du type de numéro si on a requis la validation d'un mobile uniquement
                if (mustBeMobileTypeOnly && isValid)
                {
                    if (util.GetNumberType(number) != PhoneNumberType.MOBILE && util.GetNumberType(number) != PhoneNumberType.FIXED_LINE_OR_MOBILE)
                        isValid = false;
                }

                return isValid;
            }
            catch (NumberParseException)
            {
                return false;
            }
        }
    }
}
