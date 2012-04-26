using System.IO;

namespace iGL.Engine.BitmapFont
{
  public struct Page
  {
    #region  Public Constructors

    public Page(int id, string fileName)
      : this()
    {
      this.FileName = fileName;
      this.Id = id;
    }

    #endregion  Public Constructors

    #region  Public Methods

    public override string ToString()
    {
      return string.Format("{0} ({1})", this.Id, Path.GetFileName(this.FileName));
    }

    #endregion  Public Methods

    #region  Public Properties

    public string FileName { get; set; }

    public int Id { get; set; }

    #endregion  Public Properties
  }
}
