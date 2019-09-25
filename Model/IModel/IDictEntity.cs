using System;
using System.Collections.Generic;
using System.Text;

namespace IModel
{
    public interface IDictEntity<T>
    {
        T DictKey { get; set; }
    }

    public interface IDictEntity: IDictEntity<string>
    {

    }
}
