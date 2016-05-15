module.exports = AdTagModal = React.createClass
  displayName: 'AdTagModal'

  propTypes:
    ad: React.PropTypes.object

  componentDidMount: ->
    @setState
      show: @props.show


  render: ->
    ad = @props.ad
    return <div/> if ad == null

    onClose = @props.onClose

    urlAdTag = ad.adTagUrl
    urlAdClick = ad.clickUrl
    urlAdImpression = ad.impressionUrl

    urlAdResponses = '/api/campaigns/' + ad.campaignId + '/ad-responses/' + ad.id + '/preview'

    Modal = Bootstrap.Modal

    return (
      <Modal id="modal-tags" show={true} onHide={onClose}>
        <Modal.Body>

          <label className="control-label">Ad Tag:</label>
          <a className="tag-preview" href={urlAdResponses} target="_blank"> Preview Ad Responses</a>
          <a className="tag-preview" href={urlAdTag} target="_blank">Preview  </a>
          <div className="form-group row">
            <div className='col-sm-12'>
              <input type="text" className="form-control" id="url-ad-tag" value={urlAdTag} onmouseup="this.select()" />
            </div>
          </div>

          <label className="control-label">Impression URL:</label>
          <div className="form-group row">
            <div className='col-sm-12'>
              <input type="text" className="form-control" id="url-ad-impression" value={urlAdImpression} onmouseup="this.select()" />
            </div>
          </div>

          <label className="control-label">Click URL:</label>
          <div className="form-group row">
            <div className='col-sm-12'>
              <input type="text" className="form-control" id="url-ad-click" value={urlAdClick} onmouseup="this.select()" />
            </div>
          </div>

        </Modal.Body>
        <Modal.Footer>
          <Bootstrap.Button bsStyle='success' onClick={onClose} style={marginRight: 20}>Close</Bootstrap.Button>
        </Modal.Footer>
      </Modal>
    )
