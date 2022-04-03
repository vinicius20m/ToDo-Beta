import Card from "./Layout/Card.jsx"
import Item from "../Components/Item.jsx"

export default (props) => {

    const titleInput = React.useRef([]);
    const descriptionInput = React.useRef([]);
    const moveInput = React.useRef([]);

    const [title, setTitle] = React.useState('')
    const [description, setDescription] = React.useState('')

    const [itens, setItens] = React.useState([]) ;

    function fetchItens(text){

        axios.get(apiAdress+'/api/Itens/'+props.quadro.id+'/'+token).then((response) => {
            
            setItens(response.data)
            console.log(text)
        })
        .catch(function (error) {
            console.log(error);
        })
    }

    function newItem(){

        if(title !== ""){

            axios.post(apiAdress+'/api/Itens'+'/'+token, {
                "title": title,
                "description": description,
                "done": false,
                "quadroId": props.quadro.id,
                "authorId": gUserId
            })
            .then(function (response) {

                setItens([...itens, {
                    id: response.data.id,
                    title: response.data.title,
                    done: false,
                    description: response.data.description,
                    quadroId: response.data.quadroId
                }])
                console.log("sucesso em criar o novo Item Id: " + response.data.id)
            })
            .catch(function (error) {
                console.log(error);
            })

            setTitle('')
            setDescription('')
        }else{
            alert('O Titulo não pode ser vazio')
        }
    }

    function editItem(c, id){

        axios.put(apiAdress+'/api/Itens/'+id+'/'+token, {
            "id": id,
            "title": titleInput.current[c].value,
            "description": descriptionInput.current[c].value,
            "quadroId": props.quadro.id,
            "authorId": gUserId
        })
        .then(() => {

            fetchItens("sucesso em editar o Item Id: " + id)
        })
        .catch(function (error) {
            console.log(error);
        })
    }

    function deleteItem(id){

        axios.delete(apiAdress+'/api/Itens/'+id+'/'+token).then((response) => {
            
            fetchItens("sucesso em remover o Item Id :" + id)
        })
        .catch(function (error) {
            console.log(error);
        })
    }

    function moveItem(c, item){

        axios.put(apiAdress+'/api/Itens/'+item.id+'/'+token, {
            "id": item.id,
            "title": item.title,
            "description": item.description,
            "quadroId": moveInput.current[c].value,
            "authorId": gUserId
        })
        .then(() => {

            props.moved("sucesso em mover o Item Id: " + item.id + ", para o Quadro Id: " + moveInput.current[c].value)
        })
        .catch(function (error) {
            console.log(error);
        })
    }

    //  UseEffect
    React.useEffect(() => {
        titleInput.current = titleInput.current.slice(0, itens.length);
        descriptionInput.current = descriptionInput.current.slice(0, itens.length);
        moveInput.current = moveInput.current.slice(0, itens.length);
    }, [itens]);

    React.useEffect(async () => {
        
        fetchItens(props.move)
    }, [props.move])  
    //

    return (
        <>
            <div className="row col-md-12">
                <div className="col-md-6">
                    <label> Titulo <input placeholder="Crie um novo Item" onChange={(e) => { setTitle(e.target.value)} } className="form-control" type="text" value={title} /> </label>
                </div>
                <div className="col-md-6">
                    <label> Descrição <input placeholder="Crie um novo Item" onChange={(e) => { setDescription(e.target.value)} } className="form-control" type="text" value={description} /> </label>
                    <button className="btn btn-success" style={{marginLeft: 110, marginBottom: 6}} onClick={newItem}>Novo Item</button>
                </div>
            </div>
            <hr />
            <div style={{display: "flex", flexWrap: "wrap"}}>

            {itens.map((item, c) => {
                
            return <div className="Item" style={{flex: 1}} key={c}>

                <Card tipo="item" moveTarget={"#moveItemModal_"+props.quadro.id+"-"+c} target={"#editItemModal_"+props.quadro.id+"-"+c} titulo={item.title}>

                    <center> <button style={{float: "right", marginRight: 20}} className="btn btn-danger" data-toggle="modal" data-target={"#deleteItemModal_"+props.quadro.id+"-"+c} >
                        <strong>x</strong></button> 
                    </center>

                    <Item item={item}></Item>
                </Card>

                <div className="modal fade" id={"deleteItemModal_"+props.quadro.id+"-"+c} tabIndex="-1" role="dialog" aria-hidden="true">
                    <div className="modal-dialog modal-dialog-centered" role="document">
                        <div className="modal-content">
                        <div style={{background: "#b54c4c"}} className="modal-header">
                            <h4 className="modal-title" id="modalLongTitle">Excluindo o Item: <strong>{item.title}</strong></h4>
                            <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div className="modal-body">
                            
                            <h2 className="text-center">Tem certeza que deseja excluir este Item?</h2>
                            <p className="text-center">Para manter o Item no Histórico, marque-o como Feito</p>

                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" data-dismiss="modal">Não</button>
                            <button 
                                type="button" 
                                className="btn btn-danger" 
                                data-dismiss="modal" 
                                onClick={() => deleteItem(item.id)}
                                >Sim</button>
                        </div>
                        </div>
                    </div>
                </div>

                <div className="modal fade" id={"editItemModal_"+props.quadro.id+"-"+c} tabIndex="-1" role="dialog" aria-hidden="true">
                    <div className="modal-dialog modal-dialog-centered" role="document">
                        <div style={{minWidth: 600}} className="modal-content">
                        <div className="modal-header">
                            <h4 className="modal-title" id="modalLongTitle">Editando o Item: <strong>{item.title}</strong></h4>
                            <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div style={{minHeight: 250}} className="modal-body">
                            
                            <label style={{minWidth: 270}} > Titulo
                                <input style={{fontWeight: "bold"}} className="form-control" type="text" defaultValue={item.title} ref={el => titleInput.current[c] = el} />
                            </label>
                            <br />
                            <label style={{minWidth: 400}} > Descrição
                                <textarea style={{fontWeight: "bold", minHeight: 100}} className="form-control" type="text" defaultValue={item.description} ref={el => descriptionInput.current[c] = el} />
                            </label>

                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" data-dismiss="modal">Fechar</button>
                            <button type="button" className="btn btn-primary" data-dismiss="modal" onClick={() => editItem(c, item.id)}>Salvar</button>
                        </div>
                        </div>
                    </div>
                </div>

                <div className="modal fade" id={"moveItemModal_"+props.quadro.id+"-"+c} tabIndex="-1" role="dialog" aria-hidden="true">
                    <div className="modal-dialog modal-dialog-centered" role="document">
                        <div className="modal-content">
                        <div className="modal-header">
                            <h4 className="modal-title" id="modalLongTitle">Movendo o Item: <strong>{item.title}</strong></h4>
                            <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div style={{background: "#F4F6F9"}} className="modal-body">

                            <label>
                                <h4 style={{}}><strong>Escolha o Quadro que vai receber este Item</strong></h4>
                                <select style={{fontWeight: "bold"}} className="form-control" ref={el => moveInput.current[c] = el}>
                                    {props.quadros.map((quadro, d) => {
                                        return <option style={{fontWeight: "bold"}} key={"move-"+d} value={quadro.id}>{quadro.title}</option>
                                    })}
                                    
                                </select>
                            </label>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" data-dismiss="modal">Fechar</button>
                            <button type="button" className="btn btn-primary" data-dismiss="modal" onClick={() => {moveItem(c, item)}}>Mover</button>
                        </div>
                        </div>
                    </div>
                </div>
            </div>
            })}
            </div>
        </>
    )
}