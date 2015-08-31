using System;
using System.Collections.Generic;
using System.Linq;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using App_Dominio.Enumeracoes;
using DWM.Models.Entidades;
using DWM.Models.Repositories;
using System.IO;
using App_Dominio.Security;

namespace DWM.Models.Persistence
{
    public class EsteiraContabilizacaoModel : CrudModel<EsteiraContabilizacao, EsteiraContabilizacaoViewModel, ApplicationContext>
    {
        #region Constructor
        public EsteiraContabilizacaoModel() { }
        public EsteiraContabilizacaoModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudModel
        public override EsteiraContabilizacaoViewModel AfterInsert(EsteiraContabilizacaoViewModel value)
        {
            #region Move o arquivo de documento 
            try
            {
                #region Check if has file to transfer from Temp Folder to Users_Data Folder
                if (value.arquivo != null && value.arquivo != "")
                {
                    #region Move the file from Temp Folder to Users_Data Folder
                    System.IO.FileInfo f = new System.IO.FileInfo(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Temp"), value.arquivo));
                    f.MoveTo(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Users_Data"), value.arquivo));
                    #endregion
                }
                #endregion
            }
            catch (DirectoryNotFoundException ex)
            {
                value.mensagem.Code = 17;
                value.mensagem.Message = MensagemPadrao.Message(17).ToString();
                value.mensagem.MessageBase = new App_DominioException(ex.InnerException != null ? ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message : ex.Message, GetType().FullName).Message + ". Path de armazenamento do arquivo de boleto/comprovante não encontrado";
                value.mensagem.MessageType = MsgType.ERROR;
            }
            catch (FileNotFoundException ex)
            {
                value.mensagem.Code = 17;
                value.mensagem.Message = MensagemPadrao.Message(17).ToString();
                value.mensagem.MessageBase = new App_DominioException(ex.InnerException != null ? ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message : ex.Message, GetType().FullName).Message + ". Arquivo de boleto/comprovante não encontrado";
                value.mensagem.MessageType = MsgType.ERROR;
            }
            catch (IOException ex)
            {
                value.mensagem.Code = 17;
                value.mensagem.Message = MensagemPadrao.Message(17).ToString();
                value.mensagem.MessageBase = new App_DominioException(ex.InnerException != null ? ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message : ex.Message, GetType().FullName).Message + ". Erro referente ao arquivo de boleto/comprovante";
                value.mensagem.MessageType = MsgType.ERROR;
            }
            catch (Exception ex)
            {
                value.mensagem.Code = 17;
                value.mensagem.Message = MensagemPadrao.Message(17).ToString();
                value.mensagem.MessageBase = new App_DominioException(ex.InnerException.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                value.mensagem.MessageType = MsgType.ERROR;
            }
            #endregion

            return base.AfterInsert(value);
        }

        public override EsteiraContabilizacaoViewModel AfterDelete(EsteiraContabilizacaoViewModel value)
        {
            #region Exclui o arquivo de documento
            try
            {
                #region Check if has file to Delete from Users_Data Folder
                if (value.arquivo != null && value.arquivo != "")
                {
                    #region Delete the file from Users_Data Folder
                    System.IO.FileInfo f = new System.IO.FileInfo(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Users_Data"), value.arquivo));
                    f.Delete();
                    #endregion
                }
                #endregion
            }
            catch (DirectoryNotFoundException ex)
            {
                value.mensagem.Code = 17;
                value.mensagem.Message = MensagemPadrao.Message(17).ToString();
                value.mensagem.MessageBase = new App_DominioException(ex.InnerException != null ? ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message : ex.Message, GetType().FullName).Message + ". Path de armazenamento do arquivo de boleto/comprovante não encontrado";
                value.mensagem.MessageType = MsgType.ERROR;
            }
            catch (FileNotFoundException ex)
            {
                value.mensagem.Code = 17;
                value.mensagem.Message = MensagemPadrao.Message(17).ToString();
                value.mensagem.MessageBase = new App_DominioException(ex.InnerException != null ? ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message : ex.Message, GetType().FullName).Message + ". Arquivo de boleto/comprovante não encontrado";
                value.mensagem.MessageType = MsgType.ERROR;
            }
            catch (IOException ex)
            {
                value.mensagem.Code = 17;
                value.mensagem.Message = MensagemPadrao.Message(17).ToString();
                value.mensagem.MessageBase = new App_DominioException(ex.InnerException != null ? ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message : ex.Message, GetType().FullName).Message + ". Erro referente ao arquivo de boleto/comprovante";
                value.mensagem.MessageType = MsgType.ERROR;
            }
            catch (Exception ex)
            {
                value.mensagem.Code = 17;
                value.mensagem.Message = MensagemPadrao.Message(17).ToString();
                value.mensagem.MessageBase = new App_DominioException(ex.InnerException.InnerException.Message ?? ex.Message, GetType().FullName).Message;
                value.mensagem.MessageType = MsgType.ERROR;
            }
            #endregion

            return base.AfterDelete(value);
        }

        public override EsteiraContabilizacao MapToEntity(EsteiraContabilizacaoViewModel value)
        {
            EsteiraContabilizacao ec = Find(value);

            if (ec == null)
            {
                ec = new EsteiraContabilizacao();
                ec.esteiraId = value.esteiraId;
                ec.arquivo = value.arquivo;
            }

            ec.nome_original = value.nome_original;

            return ec;
        }

        public override EsteiraContabilizacaoViewModel MapToRepository(EsteiraContabilizacao entity)
        {
            return new EsteiraContabilizacaoViewModel()
            {
                esteiraId = entity.esteiraId,
                arquivo = entity.arquivo,
                nome_original = entity.nome_original,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override EsteiraContabilizacao Find(EsteiraContabilizacaoViewModel key)
        {
            return db.Arquivos.Find(key.esteiraId, key.arquivo);
        }

        public override Validate Validate(EsteiraContabilizacaoViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (operation == Crud.ALTERAR || operation == Crud.EXCLUIR && value.esteiraId == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Esteira ID").ToString();
                value.mensagem.MessageBase = "Identificador da esteira deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (operation == Crud.ALTERAR || operation == Crud.EXCLUIR && (value.arquivo == null || value.arquivo == ""))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Arquivo").ToString();
                value.mensagem.MessageBase = "Arquivo deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.nome_original == null || value.nome_original == "")
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome do arquivo").ToString();
                value.mensagem.MessageBase = "Nome do arquivo deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            return value.mensagem;
        }
        #endregion
    }

    public class ListViewEsteiraContabilizacao : ListViewModel<EsteiraContabilizacaoViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewEsteiraContabilizacao() { }
        public ListViewEsteiraContabilizacao(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<EsteiraContabilizacaoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _esteiraId = int.Parse(param[0].ToString());

            return (from arq in db.Arquivos
                    join est in db.Esteiras on arq.esteiraId equals est.esteiraId
                    where est.propostaId == (from e in db.Esteiras where e.esteiraId == _esteiraId select e.propostaId).FirstOrDefault()
                    orderby arq.arquivo
                    select new EsteiraContabilizacaoViewModel()
                    {
                        esteiraId = est.esteiraId,
                        arquivo = arq.arquivo,
                        nome_original = arq.nome_original,
                        PageSize = pageSize,
                        TotalCount = (from arq1 in db.Arquivos
                                      join est1 in db.Esteiras on arq1.esteiraId equals est1.esteiraId
                                      where est1.propostaId == (from e1 in db.Esteiras where e1.esteiraId == _esteiraId select e1.propostaId).FirstOrDefault()
                                      orderby arq1.arquivo
                                      select est1.esteiraId).Count()
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new EsteiraContabilizacaoModel().getObject((EsteiraContabilizacaoViewModel)id);
        }

        public override string action()
        {
            return "../Workflow/ListArquivos";
        }

        public override string DivId()
        {
            return "dados_proposta";
        }

        #endregion
    }
}