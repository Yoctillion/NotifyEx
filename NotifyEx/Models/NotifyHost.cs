using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper;
using Nekoxy;
using NotifyEx.Models.NotifyType;

namespace NotifyEx.Models
{
    public class NotifyHost
    {
        public static NotifyHost Current { get; internal set; }

        public static event Action InstanceInitialized;

        internal static void TryInitialize(Plugin notifier)
        {
            if (Current == null)
            {
                Current = new NotifyHost(notifier);
                InstanceInitialized?.Invoke();
            }
        }


        private readonly Plugin _notifier;

        private NotifyHost(Plugin notifier)
        {
            this._notifier = notifier;
        }


        #region kcapi Notify

        #region NotifyTypeKey

        private class NotifyTypeKey : IEquatable<NotifyTypeKey>
        {
            internal string Uri { get; }
            internal INotifyType Type { get; }

            internal NotifyTypeKey(string uri, INotifyType type)
            {
                this.Uri = uri;
                this.Type = type;
            }

            public bool Equals(NotifyTypeKey other)
            {
                return this.Uri == other.Uri && this.Type.Equals(other.Type);
            }

            public override bool Equals(object obj)
            {
                var nk = obj as NotifyTypeKey;
                return nk?.Equals(this) ?? false;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Uri?.GetHashCode() ?? 0) * 397) ^ (Type?.GetHashCode() ?? 0);
                }
            }
        }

        #endregion

        private readonly Dictionary<NotifyTypeKey, Func<Session, string>> _notifyProviders = new Dictionary<NotifyTypeKey, Func<Session, string>>();

        ///// <summary>
        ///// 注册通知操作
        ///// </summary>
        ///// <param name="kcapi">监视的API</param>
        ///// <param name="type">通知类型</param>
        ///// <param name="notifyProvider">return null or "" => 不进行通知</param>
        public static void Register(string kcapi, INotifyType type, Func<Session, string> notifyProvider)
        {
            if (Current != null)
            {
                Current._Register(kcapi, type, notifyProvider);
            }
            else
            {
                InstanceInitialized += () => Current._Register(kcapi, type, notifyProvider);
            }
        }

        private void _Register(string kcapi, INotifyType type, Func<Session, string> notifyProvider)
        {
            var key = new NotifyTypeKey(kcapi, type);

            Func<Session, string> action;
            if (this._notifyProviders.TryGetValue(key, out action))
            {
                action += notifyProvider;
                this._notifyProviders[key] = action;
            }
            else
            {
                this._notifyProviders.Add(key, notifyProvider);

                KanColleClient.Current.Proxy.ApiSessionSource
                    .Where(s => s.Request.PathAndQuery == kcapi)
                    .Subscribe(s => Current.Notify(s, key));
            }
        }


        private void Notify(Session session, NotifyTypeKey key)
        {
            var action = this._notifyProviders[key];
            var notifications = action.GetInvocationList()
                .Select(f => ((Func<Session, string>)f)(session))
                .Where(x => !string.IsNullOrEmpty(x));

            this._notifier.Notify(key.Uri, key.Type.Name, string.Join(Environment.NewLine, notifications));
        }

        #endregion


        public void Notify(string type, string header, string body)
        {
            Current?._notifier.Notify(type, header, body);
        }
    }
}
