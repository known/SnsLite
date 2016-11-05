using System;

namespace Known
{
    /// <summary>
    /// 分页信息。
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// 构造函数，创建一个默认每页大小为10的实例。
        /// </summary>
        public Pagination()
        {
            PageSize = 10;
        }

        /// <summary>
        /// 取得或设置分页数据的总记录数。
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 取得或设置每页大小，默认为10。
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 取得或设置当前页码。
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 取得分页数据的总页数。
        /// </summary>
        public int PageCount
        {
            get { return (int)Math.Ceiling(TotalCount / (double)PageSize); }
        }

        /// <summary>
        /// 取得是否有上一页。
        /// </summary>
        public bool HasPrevious
        {
            get { return PageIndex != 1; }
        }

        /// <summary>
        /// 取得是否有下一页。
        /// </summary>
        public bool HasNext
        {
            get { return PageIndex != PageCount; }
        }
    }
}
