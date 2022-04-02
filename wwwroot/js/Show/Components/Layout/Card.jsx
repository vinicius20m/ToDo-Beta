

export default props =>
<>

<div style={{

    minWidth: props.min_width ? props.min_width : false,
    maxWidth: props.max_width ? props.max_width : false, 
    minHeight: props.min_height ? props.min_height : false, 
    maxHeight: props.max_height ? props.max_height : false
}} className="Card">

    <div style={{marginLeft: 0}} className="Header row col-md-12">

        <div className="col-md-11" data-toggle="modal" data-target={props.target? props.target : false}>

            <strong style={{marginLeft: "8%"}}>{props.titulo}</strong>
        </div>

        
        {props.tipo == "item" && 
            <div data-toggle="modal" data-target={props.moveTarget} className="col-md-1 moveItem">
                <a><i className="fas fa-arrow-circle-right"></i></a>
            </div>
        }
    </div>

    <div className="Conteudo">
        {props.children}
    </div>
</div>
</>