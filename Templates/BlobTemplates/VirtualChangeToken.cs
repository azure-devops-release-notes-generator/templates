using System;
using Common.Options;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Templates.BlobTemplates
{
    public class VirtualChangeToken : IChangeToken
    {
        private readonly Func<Option<DateTimeOffset>> _currentModifiedDateGetter;
        private readonly Func<Option<IFileInfo>> _fileInfoGetter;
        public VirtualChangeToken(Func<Option<DateTimeOffset>> currentModifiedDateGetter, Func<Option<IFileInfo>> fileInfoGetter)
        {
            _currentModifiedDateGetter = currentModifiedDateGetter ?? throw new ArgumentNullException(nameof(currentModifiedDateGetter));
            _fileInfoGetter = fileInfoGetter ?? throw new ArgumentNullException(nameof(fileInfoGetter));
        }
        public bool ActiveChangeCallbacks => false;
        public bool HasChanged =>
                                 _currentModifiedDateGetter()
                                .Map(currentModifiedDate => _fileInfoGetter()
                                                                            .Map(x => currentModifiedDate < x.LastModified)
                                                                            .Reduce(true)
                                    )
                                .Reduce(true);
        public IDisposable RegisterChangeCallback(Action<object> callback, object state) => Noop.Instance;

        public class Noop : IDisposable
        {
            public static IDisposable Instance = new Noop();
            private Noop() { }
            public void Dispose() { }
        }
    }
}
