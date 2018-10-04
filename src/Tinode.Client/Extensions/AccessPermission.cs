using System;

namespace Tinode.Client.Extensions
{
    public static class AccessPermissionExtensions
    {
        public static unsafe string AsString(this AccessPermission acs)
        {
            if (acs == 0) return string.Intern("N");

            var x = (int) acs;

            var chars = stackalloc char[8];

            var i = 0;

            chars[i] = (x & (int) AccessPermission.Join) == (int) AccessPermission.Join && ++i != 0 ? 'J' : chars[i];
            chars[i] = (x & (int) AccessPermission.Read) == (int) AccessPermission.Read && ++i != 0 ? 'R' : chars[i];
            chars[i] = (x & (int) AccessPermission.Write) == (int) AccessPermission.Write && ++i != 0 ? 'W' : chars[i];
            chars[i] = (x & (int) AccessPermission.Presence) == (int) AccessPermission.Presence && ++i != 0 ? 'P' : chars[i];
            chars[i] = (x & (int) AccessPermission.Approve) == (int) AccessPermission.Approve && ++i != 0 ? 'A' : chars[i];
            chars[i] = (x & (int) AccessPermission.Sharing) == (int) AccessPermission.Sharing && ++i != 0 ? 'S' : chars[i];
            chars[i] = (x & (int) AccessPermission.Delete) == (int) AccessPermission.Delete && ++i != 0 ? 'D' : chars[i];
            chars[i] = (x & (int) AccessPermission.Owner) == (int) AccessPermission.Owner && ++i != 0 ? 'O' : chars[i];


            return new string(chars, 0, i);
        }

        public static AccessPermission AsAccessPermission(this string acs)
        {
            if (string.IsNullOrEmpty(acs)) return AccessPermission.No;

            var x = 0;

            for (var i = 0; i < acs.Length; i++)
            {
                switch (acs[i])
                {
                    case 'J':
                    {
                        x = x | (int) AccessPermission.Join;
                        break;
                    }
                    case 'R':
                    {
                        x = x | (int) AccessPermission.Read;
                        break;
                    }
                    case 'W':
                    {
                        x = x | (int) AccessPermission.Write;
                        break;
                    }
                    case 'P':
                    {
                        x = x | (int) AccessPermission.Presence;
                        break;
                    }
                    case 'A':
                    {
                        x = x | (int) AccessPermission.Approve;
                        break;
                    }
                    case 'S':
                    {
                        x = x | (int) AccessPermission.Sharing;
                        break;
                    }
                    case 'D':
                    {
                        x = x | (int) AccessPermission.Delete;
                        break;
                    }
                    case 'O':
                    {
                        x = x | (int) AccessPermission.Owner;
                        break;
                    }

                    default:
                        throw new ArgumentException($"invalid char '{acs[i]}' in accessPersmission string");
                }
            }

            return (AccessPermission) x;
        }
    }
}