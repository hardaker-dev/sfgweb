export class Content {

  constructor(
    public id: string,
    public value: string,
    public locale: string,
    public contentType: string
  ) {
  }

}


//public class Activity {
//  public int? Id { get; set; }
//        public string Name { get; set; }
//        public IList < Content > TitleContent { get; set; }
//        public IList < Content > DescriptionContent { get; set; }
//        public IList < Content > MenuImageContent { get; set; }
//        public IList < Content > VideoContent { get; set; }
//        public IList < Content > ImageContent { get; set; }
//        public bool ? IsActive { get; set; }
//        public string CategoryName { get; set; }

//        public IList < ActivitySku > Skus { get; set; }
//        public IList < ActivityEquiptment > Equiptment { get; set; }
//    }
