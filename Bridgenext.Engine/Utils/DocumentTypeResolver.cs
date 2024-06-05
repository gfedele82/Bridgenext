using Bridgenext.Engine.Interfaces;
using Bridgenext.Models.Enums;

namespace Bridgenext.Engine.Utils
{
    public delegate IProcessDocumentByType DocumentTypeResolver(FileTypes fileTypes);
}
