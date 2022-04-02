import Quadro from "./Components/Quadro.jsx"
import Card from "./Components/Layout/Card.jsx"

export default () => {

    const titleInput = React.useRef([]);
    const descriptionInput = React.useRef([]);

    const [title, setTitle] = React.useState('')
    const [description, setDescription] = React.useState('')

    const [quadros, setQuadros] = React.useState([]) ;

    const [moved, setMoved] = React.useState("empty")

    

    function fetchQuadros(text){

        axios.get(apiAdress+'/api/Quadros/'+gAmbienteId+'/'+token).then((response) => {                  
            
            setQuadros(response.data)
            console.log(text)
        })
        .catch(function (error) {
            console.log(error)
        })
    }

    function newQuadro(){

        if(title !== ""){

            axios.post(apiAdress+'/api/Quadros'+'/'+token, {
                "title": title,
                "description": description,
                "done": false,
                "AmbienteId": gAmbienteId
            })
            .then(function (response) {

                setQuadros([...quadros, {
                    id: response.data.id,
                    title: response.data.title,
                    description: response.data.description,
                    ambienteId: response.data.ambienteId
                }])
                console.log("success created a new Quadro Id: " + response.data.id)
            })
            .catch(function (error) {
                console.log(error)
            })

            setTitle('')
            setDescription('')
        }else{
            alert('O Titulo não pode ser vazio')
        }
    }

    function editQuadro(c, id){

        axios.put(apiAdress+'/api/Quadros/'+id+'/'+token, {
            "id": id,
            "title": titleInput.current[c].value,
            "description": descriptionInput.current[c].value,
            "ambienteId": gAmbienteId
        })
        .then(() => {

            fetchQuadros("success edited Quadro Id: " + id)
        })
        .catch(function (error) {
            console.log(error);
        })
    }

    function deleteQuadro(id){

        axios.delete(apiAdress+'/api/Quadros/'+id+'/'+token).then((response) => {

            fetchQuadros("success removed Quadro Id :" + id)
        })
        .catch(function (error) {
            console.log(error);
        })
    }

    function wasItemMoved(text){
        console.log("passou por aqui")
        setMoved(text)
    }

    //  UseEffect
    React.useEffect(() => {

        titleInput.current = titleInput.current.slice(0, quadros.length);
        descriptionInput.current = descriptionInput.current.slice(0, quadros.length);
    }, [quadros]);
    
    React.useEffect(async () => {
        
        fetchQuadros("success loaded Quadros")
        // console.log(gUserId)
    }, []) 
    //

    return (
        <>
        {/* INPUTS ----------------------------- */}
            <div id="inputRow" className="row col-md-12">
                <div className="col-md-6">
                    <label style={{marginLeft: 80}}> Titulo <input placeholder="Crie um novo Quadro" onChange={(e) => { setTitle(e.target.value)} } className="form-control" type="text" value={title} /> </label>
                </div>
                <div className="col-md-6">
                    <label> Descrição <input placeholder="Crie um novo Quadro" onChange={(e) => { setDescription(e.target.value)} } className="form-control" type="text" value={description} /> </label>
                    <button className="btn btn-success" style={{marginLeft: 110, marginBottom: 6}} onClick={newQuadro}>Novo Quadro</button>
                </div>
            </div>
            <hr /><br />

            {quadros.map((quadro, c) => {

                return <div className="Quadro" key={c}>

                    <Card tipo="quadro" target={"#editQuadroModal_"+c} min_width={900} titulo={quadro.title}>
                        <center> <button style={{float: "right", marginRight: 20}} className="btn btn-danger" data-toggle="modal" data-target={"#deleteQuadroModal_"+c} >
                            <strong>x</strong></button> 
                        </center>
                        <Quadro moved={wasItemMoved} move={moved} quadros={quadros} quadro={quadro} count={c}></Quadro>
                    </Card>

                    <div className="modal fade" id={"deleteQuadroModal_"+c} tabIndex="-1" role="dialog" aria-hidden="true">
                        <div className="modal-dialog modal-dialog-centered" role="document">
                            <div className="modal-content">
                            <div style={{background: "#b54c4c"}} className="modal-header">
                                <h4 className="modal-title" id="modalLongTitle">Excluindo o Quadro: <strong>{quadro.title}</strong></h4>
                                <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div className="modal-body">
                                
                                <h2 className="text-center">Tem certeza que deseja excluir este Quadro?</h2>
                                <p className="text-center">Para manter o Quadro no Histórico, marque-o como Feito</p>

                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-secondary" data-dismiss="modal">Não</button>
                                <button 
                                    type="button" 
                                    className="btn btn-danger" 
                                    data-dismiss="modal" 
                                    onClick={() => deleteQuadro(quadro.id)}
                                    >Sim</button>
                            </div>
                            </div>
                        </div>
                    </div>

                    <div className="modal fade" id={"editQuadroModal_"+c} tabIndex="-1" role="dialog" aria-hidden="true">
                        <div className="modal-dialog modal-dialog-centered" role="document">
                            <div style={{minWidth: 600}} className="modal-content">
                            <div style={{background: "#85abd4"}} className="modal-header">
                                <h4 className="modal-title" id="modalLongTitle">Editando o Quadro: <strong>{quadro.title}</strong></h4>
                                <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div style={{background: "#fff3b6"}} className="modal-body">
                                
                                <label style={{minWidth: 270}} > Titulo
                                    <input style={{ fontWeight: "bold"}} className="form-control" type="text" defaultValue={quadro.title} ref={el => titleInput.current[c] = el} />
                                </label>
                                <br />
                                <label style={{minWidth: 400}} > Descrição
                                    <textarea style={{ fontWeight: "bold"}} className="form-control" type="text" defaultValue={quadro.description} ref={el => descriptionInput.current[c] = el} />
                                </label>

                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-secondary" data-dismiss="modal">Fechar</button>
                                <button type="button" className="btn btn-primary" data-dismiss="modal" onClick={() => editQuadro(c, quadro.id)}>Salvar</button>
                            </div>
                            </div>
                        </div>
                    </div>
                    <hr />
                    <br />
                    <br />
                </div>
            })}

        </>
    )
}